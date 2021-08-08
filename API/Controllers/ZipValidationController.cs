using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using API.Data;
using API.DTO;
using API.Entities;
using API.Helpers;
using API.Logs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ZipValidationController : BaseApiController
    {
        Logger log = new Logger("Zip Validation Logs");

        EmailNotification emailNotification = new EmailNotification();
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ZipValidationController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("hello")]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [HttpGet("zipdetails")]
        public async Task<ActionResult<FileSetup>> ZipDetails()
        {
            return await _context.FileSetups.Include(x => x.allowedFileTypes).FirstOrDefaultAsync();
        }

        [HttpPost("create-zipvalidation")]
        public async Task<ActionResult<FileSetup>> CreateZipValidation(FileSetup fileSetup)
        {
            _context.FileSetups.Add(fileSetup);
            await _context.SaveChangesAsync();

            return new FileSetup
            {
                source = fileSetup.source,
                target = fileSetup.target,
                adminEmail = fileSetup.adminEmail,
                allowedFileTypes = fileSetup.allowedFileTypes
            };
        }

        [HttpPost("update-zipvalidation")]
        public async Task<ActionResult<FileSetup>> UpdateZipValidation(FileSetup fileSetup)
        {
            var result = await _context.FileSetups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileSetup.Id);

            if (result != null)
            {
                var exisitingRecord = _mapper.Map<FileSetup>(fileSetup);
                _context.FileSetups.Update(exisitingRecord);
            }

            FileSetup fileDetails = new FileSetup
            {
                source = fileSetup.source,
                target = fileSetup.target,
                adminEmail = fileSetup.adminEmail

            };


            await _context.SaveChangesAsync();
            return fileDetails;

        }

        [HttpPost("update-filetype")]
        public async Task<ActionResult<AllowedFileTypes>> updateFileType(AllowedFileTypes fileTypes)
        {
            var result = _context.AllowedFileTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileTypes.Id);
            if (result != null)
            {
                var exisitingRecord = _mapper.Map<AllowedFileTypes>(fileTypes);
                _context.AllowedFileTypes.Update(exisitingRecord);
            }
            else
            {
                var newSetup = _mapper.Map<AllowedFileTypes>(fileTypes);
                _context.AllowedFileTypes.Add(newSetup);

            }

            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpDelete("remove-filetype/{fileTypeId}")]
        public async Task<ActionResult> removeFileType(int fileTypeId)
        {
            var result = await _context.AllowedFileTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileTypeId);
            if (result != null)
            {

                _context.AllowedFileTypes.Remove(result);
            }

            await _context.SaveChangesAsync();
            return Ok();

        }


        [HttpPost("zip-validate")]
        public async Task<ActionResult> ZipValidate(FileSetup fileSetup)
        {
            bool IsZipExist = false;
            bool IsFileCorrupt = false;
            bool IsAllowedFileType = true;
            bool IsXmlFile = false;
            string xmlFilePath = "";
            string xsdFilePath = "";
            int applicationId = 1700017;
            Guid guid = Guid.NewGuid();


            string src = fileSetup.source;
            string tgt = $@"{fileSetup.target}\{applicationId}-{guid}";
            string eml = fileSetup.adminEmail;

            var allowedFileTypeList = await _context.AllowedFileTypes.Where(x => x.FileSetupId == fileSetup.Id).Select(y => y.FileTypeExtension).ToListAsync();
            // List<string> list = new List<string>();
            // list.Add(".docx");
            // list.Add(".pdf");
            // list.Add(".xlsx");
            // list.Add(".xml");
            // list.Add(".xsd");



            //--------logic----------//

            var files = Directory.GetFiles(src);
            foreach (var file in files)
            {
                if (file.Contains("zip"))
                {
                    log.LogWrite(@$"Valid zip file exist {file}");
                    IsZipExist = true;


                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        log.LogWrite(@$"Total files found {archive.Entries.Count}");

                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            string extension = Path.GetExtension(entry.FullName).ToLower();
                            if (!allowedFileTypeList.Contains(extension))
                            {

                                log.LogWrite(@$"File type extension -{extension} is not allowed of file {entry.FullName}");

                                emailNotification.Sendmail(@$"File type extension -{extension} is not allowed of file {entry.FullName}", eml);
                                log.LogWrite(@$"Email Sent to Administrator at -{eml}");
                            }
                            else
                            {
                                System.IO.Directory.CreateDirectory(tgt);
                                entry.ExtractToFile(Path.Combine(tgt, entry.FullName));
                                log.LogWrite(@$"File - {entry.FullName} has been extracted to {Path.Combine(tgt, entry.FullName)}");
                                if (extension.Contains(".xml"))
                                {
                                    IsXmlFile = true;
                                    xmlFilePath = Path.Combine(tgt, entry.FullName);
                                }
                                else if (extension.Contains(".xsd"))
                                {
                                    xsdFilePath = Path.Combine(tgt, entry.FullName);
                                }
                            }
                        }

                    }

                }
                else
                {

                    log.LogWrite(@$" Zip file not exist on location - {src}");

                }

            }
            if (IsXmlFile)
            {
                try
                {
                    XmlSchemaSet schema = new XmlSchemaSet();
                    schema.Add("", xsdFilePath);
                    XmlReader rd = XmlReader.Create(xmlFilePath);
                    XDocument doc = XDocument.Load(rd);
                    doc.Validate(schema, ValidationEventHandler);
                }
                catch (Exception ex)
                {
                    emailNotification.Sendmail(ex.Message, eml);
                    log.LogWrite(@$"Email Sent to Administrator at -{eml}");
                    log.LogWrite(@$"Error with XML attributes {ex.Message}");
                    return BadRequest(ex.Message);
                }


            }
            emailNotification.Sendmail(@$"File has been successfully extracted to {tgt}",eml);
            return Ok();

        }


        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error)
                {
                    throw new Exception(e.Message);
                }
            }
        }





    }


}
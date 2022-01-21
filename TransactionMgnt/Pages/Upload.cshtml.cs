using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMgnt.Pages
{
    public class UploadModel : PageModel
    {
        private readonly ILogger<UploadModel> _logger;
        [BindProperty]
        public BufferedDoubleFileUploadPhysical FileUpload { get; set; }

        public string Result { get; private set; }
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".csv", ".xml"};
        private readonly string _targetFilePath;

        public UploadModel(ILogger<UploadModel> logger)
        {
            _logger = logger;
            _fileSizeLimit = 1048576;
            _targetFilePath = "c:\\files";

        }

        public void OnGet()
        {
        
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";
                ViewData["Success"] = "";
                return Page();
            }

            var formFiles = new List<IFormFile>()
            {
                FileUpload.FormFile1
            };


            System.IO.Directory.CreateDirectory(_targetFilePath);

            foreach (var formFile in formFiles)
            {
                var formFileContent =
                    await FileHelpers
                        .ProcessFormFile<BufferedDoubleFileUploadPhysical>(
                            formFile, ModelState, _permittedExtensions,
                            _fileSizeLimit);

                if (!ModelState.IsValid)
                {
                    Result = "Please correct the form.";
                    ViewData["Success"] = "";
                    return Page();
                }

             
                var filePath = Path.Combine(
                    _targetFilePath, formFile.FileName);



                using (var fileStream = System.IO.File.Create(filePath))
                {
                    await fileStream.WriteAsync(formFileContent);

                }

                CsvHelper.Configuration.CsvConfiguration csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture);
                csvConfiguration.Delimiter = ",";
                csvConfiguration.HasHeaderRecord = false;
                csvConfiguration.BadDataFound = null;
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, csvConfiguration))
                {
                    try
                    {
                        var records = csv.GetRecords<TransactionDto>();
                        var result = records.ToList();

                        foreach (TransactionDto transactionDto in result)
                        {
                            using (var db = new DatabaseContext())
                            {
                                Transaction transaction = new Transaction();
                                transaction.Id = Guid.NewGuid();
                                transaction.TransactionId = transactionDto.TransactionId;
                                transaction.Amount = decimal.Parse(transactionDto.Amount);
                                transaction.CurrencyCode = transactionDto.CurrencyCode;
                                transaction.TransactionDate = DateTime.Parse(transactionDto.TransactionDate);
                                transaction.CreatedDateTime = DateTime.Now;
                                db.Transaction.Add(transaction);
                                db.SaveChanges();
                            }
                        }

                    }
                    catch (BadDataException ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
            }
        
            ViewData["Success"] = "File was uploaded successfully";
            return Page();           
        }

    }

    public class BufferedDoubleFileUploadPhysical
    {
        [Required]
        [Display(Name = "Upload File")]
        public IFormFile FormFile1 { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }
}

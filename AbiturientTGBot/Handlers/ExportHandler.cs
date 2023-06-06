using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using AbiturientTGBot.Service;
using AbiturientTGBot.Models;

namespace AbiturientTGBot.Handlers
{
    public class ExportHandler
    {
        string exportFolder = $"";
        DBService db;

        public ExportHandler(DBService db, string exportFolder)
        {
            this.db = db;
            this.exportFolder = exportFolder;
        }

        public async void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string excelName = string.Empty;
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                var _abiturients = GetApplications();
                workSheet.Cells.LoadFromCollection(_abiturients, true);
                excelName = $"{exportFolder}\\AbiturientList-{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.xlsx";

                try
                {
                    if (Directory.Exists(exportFolder))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(exportFolder);
                        FileInfo[] files = directoryInfo.GetFiles();

                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }

                        Console.WriteLine($"Папка {exportFolder} успешно очищена.");
                    }

                    await package.SaveAsAsync(new FileInfo(excelName));
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(
                        "Что-то пошло не так во время сохранения\nException message:" + ex.Message);
                }

                await Console.Out.WriteLineAsync($"Экспорт заявок в по пути {excelName} завершен успешно");
            }
            stream.Position = 0;
        }

        private List<Models.Application> GetApplications() 
        {
            var _abiturients = db.GetAllAbiturients();

            List<Models.Application> applications = new List<Models.Application>();

            foreach (Abiturient abiturient in _abiturients)
            {
                if (abiturient.IsFull == false)
                { 
                    continue;
                }

                int specId = (int)abiturient.SpecId;
                DateTime fillingDate = (DateTime)abiturient.FillingDate;

                applications.Add(new Models.Application
                { 
                    SpecName = db.GetSpecName(specId),
                    ClassReq = db.GetSpecClassReq(specId),
                    Surname = abiturient.Surname,
                    FirstName = abiturient.Firstname,
                    Patronymic = abiturient.Patronymic,
                    Address = abiturient.Address,
                    SchoolName = abiturient.SchoolName,
                    SchoolAddress = abiturient.SchoolAddress,
                    SchoolMark = (double)abiturient.SchoolMark,
                    IsMale = (bool)abiturient.IsMale,
                    IsOrphan = (bool)abiturient.IsOrphan,
                    InvalidGroup = (int)abiturient.InvalidGroup,
                    IsFromChernobyl = (bool)abiturient.IsFromChernobyl,
                    IsNeedHostel = (bool)abiturient.IsNeedHostel,
                    HomePhone = abiturient.HomePhone.ToString(),
                    MobilePhone = abiturient.MobilePhone.ToString(),        
                    FillingDate = fillingDate.ToString("dd-MM-yyyy HH:mm:ss")
            });
            }

            return applications;
        }

        //public void ExportToExcel()
        //{
        //    DataGridView dataGrid = new DataGridView();

        //    if (dataGridView1.Rows.Count > 0)
        //    {
        //        Microsoft.Office.Interop.Excel.Application xceApp = new Microsoft.Office.Interop.Excel.Application();
        //        xceApp.Application.Workbooks.Add(Type.Missing);
        //        for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
        //        {
        //            xceApp.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;

        //        }

        //        for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dataGridView1.Columns.Count; j++)
        //            {
        //                xceApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
        //            }
        //        }

        //        xceApp.Columns.AutoFit();
        //        xceApp.Visible = true;
        //    }
        //}

        //public void ExportToExce123l()
        //{
        //    Excel.Application xlsApp;
        //    Excel.Workbook xlsWorkbook;
        //    Excel.Worksheet xlsWorksheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    // Remove the old excel report file
        //    try
        //    {
        //        FileInfo oldFile = new FileInfo(fileName);
        //        if (oldFile.Exists)
        //        {
        //            File.SetAttributes(oldFile.FullName, FileAttributes.Normal);
        //            oldFile.Delete();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //    xlsApp = new Excel.Application();
        //    xlsWorkbook = xlsApp.Workbooks.Add(misValue);
        //    xlsWorksheet = (Excel.Worksheet)xlsWorkbook.Sheets[1];

        //    // Create the header for Excel file
        //    xlsWorksheet.Cells[1, 1] = "Example of Excel report. Get data from pubs database, table authors";
        //    Excel.Range range = xlsWorksheet.get_Range("A1", "E1");
        //    range.Merge(1);
        //    range.Borders.Color = Color.Black.ToArgb();
        //    range.Interior.Color = Color.Yellow.ToArgb(); dynamic dbschema = new JObject();


        //}
    }
}

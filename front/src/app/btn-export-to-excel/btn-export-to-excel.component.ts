import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';

@Component({
  selector: 'btn-export-to-excel',
  templateUrl: './btn-export-to-excel.component.html',
  styleUrls: ['./btn-export-to-excel.component.scss']
})
export class BtnExportToExcelComponent implements OnInit {

    //Utilizamos get/set para poder obtener los datos refrescados en el componente padre
  public dataTable;

  get dataTableInput(): any
  {
    return this.dataTable;
  }

  @Input() 
    set dataTableInput (val: any){
      this.dataTable = val;
    }

  //@Input() dataTableInput: any[]; //datos de la tabla, de esta forma no se actualiza 
                                    //a la misma vez que el componente padre.
  @Input() fieldsInput: any[]; //campos a visualizar, traducciçon y ancho de la columna
  @Input() objectTranslateInput: string; //componente del que cuelgan las traducciones

  //public dataTable: any[];
  public name: string;
  public fields: any[];
  public headerTranslate: string[];
  public objectTranslate: string;

  constructor(
    private _translateService: TranslateService
  ) {}

  ngOnInit() {
  
    this.dataTable = this.dataTableInput;
    this.fields = this.fieldsInput;
    this.objectTranslate = this.objectTranslateInput;

  }

  public ExportToExcel(){
    var woorksheet = "",title="",nombre="";

    this._translateService.get(this.objectTranslate + '.EXCEL_WORKSHEET').subscribe(value => { woorksheet = value; });
    this._translateService.get(this.objectTranslate + '.EXCEL_TITLE').subscribe(value => { title = value; });
    this._translateService.get(this.objectTranslate+ '.EXCEL_DOCUMENT_NAME').subscribe(value => { nombre = value; });
    
    let workbook = new Workbook();
    
    let worksheet = workbook.addWorksheet(woorksheet);

    for(var i = 0; i < this.fields.length; i++)
    {
      this._translateService.get(this.objectTranslate + '.' + this.fields[i][2]).subscribe(value => { this.fields[i][0] = value; });
    }

    let titleRow = worksheet.addRow([title]);
    titleRow.font = { name: 'Arial', family: 4, size: 16, bold: true }
    worksheet.addRow([]);

    var header = [];
    var field = []

    this.fields.forEach(element => {
      header.push(element[0]);
      field.push(element[1])
    });
    //Add Header Row
    let headerRow = worksheet.addRow(header);
    
    // Cell Style : Fill and Border
    headerRow.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFEEEEEE' },
        bgColor: { argb: '110000' }
      }
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } }
    })

    this.dataTable.forEach(d => {

      var rowData: any[] = [];
      var data : any;

      for(var i = 0; i < this.fields.length; i++)
      {
        switch (this.fields[i][5]) {
          case "Date":
            data = new Date(d[field[i]]);
            break;

          case "Percentage":
            data = d[field[i]]/100;
            break;
        
          default:
            data =  d[field[i]];
            break;
        }
        rowData.push(data);
      }
      
      //worksheet.addRow([new Date(d[field[0]]), d[field[1]], d[field[2]], d[field[3]], d[field[4]], d[field[5]]]);
      worksheet.addRow(rowData);
      }
    );

    for(var i = 1; i <= this.fields.length; i++)
    {
      worksheet.getColumn(i).width = this.fields[i-1][3]; //Asignamos ancho columna
      worksheet.getColumn(i).numFmt = this.fields[i-1][4]; //Asignamos formato columna      
    }

    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, nombre +'.xlsx');
    })
  }

}

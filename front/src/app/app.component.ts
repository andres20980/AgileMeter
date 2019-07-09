import { Component, HostListener } from '@angular/core';
import { StorageDataService } from './services/StorageDataService';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { ProyectoService } from './services/ProyectoService';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [StorageDataService]
})
export class AppComponent {
  public NombreDeUsuario: string = null;
  public UserLongName: string = "";
  public NombreDeProyecto: string = null;
  public RolDeUsuario: boolean = false;
  public ScreenWidth;
  public AssessmentName: string = null;

  //Para la barra de arriba
  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.ScreenWidth = window.innerWidth;
  }


  constructor(
    public _storageDataService: StorageDataService,
    public _router: Router,
    public translate: TranslateService) {
    this.ScreenWidth = window.innerWidth;

    translate.addLangs(['es', 'en']);
    translate.setDefaultLang('es');

    const browserLang = translate.getBrowserLang();
    translate.use(browserLang.match(/es|en/) ? browserLang : 'es');
  }

  public ChangeLang(lang: string) {
    this.translate.use(lang);
    this.refreshBreadCrumb();
  }


  public ComprobarUserYToken() {
    //Recogemos los datos
    var Token = this._storageDataService.TokenUser;
    var User = this._storageDataService.UserData;
    var TokenStorage = localStorage.getItem("tokenuser");
    var UserStorage = { 'nombre': localStorage.getItem("user"), 'password': localStorage.getItem("passuser") }

    //Recoger del localstorage tambien
    //Si no tiene nada nos devuelve a login sino nos da el token
    if (Token == null || Token == undefined || User == null || User == undefined || Token == "" || User.nombre == "" || User.password == "") {
      TokenStorage = localStorage.getItem("tokenuser");
      UserStorage = { 'nombre': localStorage.getItem("user"), 'password': localStorage.getItem("passuser") };
      if (TokenStorage == null || TokenStorage == undefined || UserStorage == null || UserStorage == undefined || TokenStorage == "" || UserStorage.nombre == "" || UserStorage.password == "") {
        this._router.navigate(["/login"]);
      } else {
        return "Bearer " + TokenStorage;
      }
    } else {
      return "Bearer " + this._storageDataService.TokenUser;
    }
  }

  //Para añadir texto a la barra de arriba
  public anadirUserProyecto(nomUsu: string, userlongname: string, nomProy: string, assessmentName?: string) {
    if (nomUsu != null) {
      this.NombreDeUsuario = nomUsu;
    }
    if (userlongname != null) {
      this.UserLongName = userlongname;
    }
    this.NombreDeProyecto = nomProy;

    this.AssessmentName = assessmentName;
  }


  public pushBreadcrumb(_var: string, _path: string) {
    var bc = this._storageDataService.breadcrumbList.find(x => x.var == _var && x.path == _path)

    if (bc != null) {
      let index: number = this._storageDataService.breadcrumbList.indexOf(bc);
      this.popBreadcrumb(index);
    }
    let _name = "";
    if (_var.length > 0) {//sin esta condicion da un error al recargar la pagina pues intenta traducir una variable no existente
      this.translate.get(_var).subscribe((res: string) => {
        _name = res;
        this._storageDataService.breadcrumbList.push({ name: _name, var: _var, path: _path });
      });
    }
  }

  public popBreadcrumb(index: number) {
    let length: number = this._storageDataService.breadcrumbList.length;
    for (var i = index; i < length; i++) {
      this._storageDataService.breadcrumbList.pop();
    }
  }

  public getBreadcrumb(index: number): any {
    return this._storageDataService.breadcrumbList[index];
  }

  public refreshBreadCrumb() {
    this._storageDataService.breadcrumbList.forEach((br, index) => {
      this.translate.get(br.var).subscribe((res: string) => {
        br.name = res;
      });
    });
  }
}

/* Suerte XD */


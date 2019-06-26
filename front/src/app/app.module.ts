import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { LoadingComponent } from './loading/loading.component';
import { AppRoutingModule, routing } from './app-routing.module';
import { HomeComponent } from './home/home.component';
import { NewevaluationComponent } from './newevaluation/newevaluation.component';
import { PreviousevaluationComponent } from './previousevaluation/previousevaluation.component';
import { MenunewevaluationComponent } from './menunewevaluation/menunewevaluation.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PdfgeneratorComponent } from './pdfgenerator/pdfgenerator.component';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { RequestInterceptorService } from './services/RequestInterceptor.service';
import { BackOfficeComponent } from './back-office/back-office.component';
import { UserManagementComponent } from './back-office/components/user-management/user-management.component';
import { AddUserProjectComponent } from './back-office/components/add-user-project/add-user-project.component';
import { SortedTableComponent } from './sorted-table/sorted-table.component';
import { PreguntasTableComponent } from './preguntas-table/preguntas-table.component';
import { MatTableModule, MatPaginatorModule, MatSortModule, MatInputModule } from '@angular/material';
import { MatFormFieldModule, MatSelectModule } from '@angular/material';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatListModule } from '@angular/material/list';
import { DebounceDirective } from './debounceDirective';
import { QuestionsManagerComponent } from './back-office/components/questions-manager/questions-manager.component';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { ComentariosTableComponent } from './comentarios-table/comentarios-table.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { PendingEvaluationComponent } from './pendingevaluation/pendingevaluation.component';
import { PendingEvaluationTableComponent } from './pendingevaluation/pendingevaluation-table/pendingevaluation-table.component';
import { BtnFinalizeEvaluationComponent } from './btn-finalize-evaluation/btn-finalize-evaluation.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { SectionResultsComponent } from './pdfgenerator/section-results/section-results.component';
import { TeamsManagerComponent } from './back-office/components/teams-manager/teams-manager.component';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { AddTeamComponent } from './back-office/components/teams-manager/add-team/add-team.component';
import { AddUpdateUserComponent } from './back-office/components/user-management/add-update-user/add-update-user.component';
import { UserListComponent } from './back-office/components/user-management/user-list/user-list.component';
import {MatSidenavModule} from '@angular/material/sidenav';

//Add custom paginator
import { MatPaginatorIntl } from '@angular/material';
import { MatPaginationIntlService } from './services/MatPaginationIntlService ';

import { TeamManagementComponent } from './back-office/components/user-management/team-management/team-management.component';

import { DragDropModule } from '@angular/cdk/drag-drop';
import 'hammerjs';

import { TranslateModule, TranslateService, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClientTrans } from './translateHttp';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    BackOfficeComponent,
    NewevaluationComponent,
    PreviousevaluationComponent,
    PendingEvaluationComponent,
    MenunewevaluationComponent,
    PdfgeneratorComponent,
    LoadingComponent,
    UserManagementComponent,
    AddUserProjectComponent,
    SortedTableComponent,
    PreguntasTableComponent,
    ComentariosTableComponent,
    PendingEvaluationTableComponent,
    DebounceDirective,
    QuestionsManagerComponent,
    BtnFinalizeEvaluationComponent,
    SectionResultsComponent,
    TeamsManagerComponent,
    BreadcrumbComponent,
    AddTeamComponent,
    AddUpdateUserComponent,
    UserListComponent,
    TeamManagementComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    routing,
    HttpModule,
    ChartsModule,
    NgbModule.forRoot(),
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    NgxMatSelectSearchModule,
    MatListModule,
    NgCircleProgressModule,
    MatExpansionModule,
    MatCheckboxModule,
    MatIconModule,
    MatSidenavModule,
    DragDropModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClientTrans]
      }
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptorService,
      multi: true
    },
    {
      provide: MatPaginatorIntl,
      useFactory: (translateService) => {
        const service = new MatPaginationIntlService();
        service.injectTranslateService(translateService);
        return service;
      }, deps: [TranslateService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { MenunewevaluationComponent } from './menunewevaluation/menunewevaluation.component';
import { NewevaluationComponent } from './newevaluation/newevaluation.component';
import { PreviousevaluationComponent } from './previousevaluation/previousevaluation.component';
import { PendingevaluationComponent } from './pendingevaluation/pendingevaluation.component';
import { PdfgeneratorComponent } from './pdfgenerator/pdfgenerator.component';
import { BackOfficeComponent } from './back-office/back-office.component';
import { UserManagementComponent } from './back-office/components/user-management/user-management.component';
import { AddUserProjectComponent } from './back-office/components/add-user-project/add-user-project.component';
import { TeamsManagerComponent } from './back-office/components/teams-manager/teams-manager.component';
import { QuestionsManagerComponent } from './back-office/components/questions-manager/questions-manager.component';
import { AddTeamComponent } from './back-office/components/teams-manager/add-team/add-team.component';
import { AddUpdateUserComponent } from './back-office/components/user-management/add-update-user/add-update-user.component';
import { TeamManagementComponent } from './back-office/components/user-management/team-management/team-management.component'
import { UserListComponent } from './back-office/components/user-management/user-list/user-list.component';
import { AboutComponent } from './about/about.component';
import { EditUserComponent } from './edit-user/edit-user.component';

const appRoutes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'editUser', component: EditUserComponent },
  {
    path: 'backoffice', component: BackOfficeComponent,
    children: [
      //{ path: 'usermanagement', component: UserManagementComponent },
      { path: 'usermanagement', component: UserListComponent },
      { path: 'usermanagement/addUser', component: AddUpdateUserComponent },
      { path: 'usermanagement/teamsManagement', component: TeamManagementComponent },
      { path: 'adduserproject', component: AddUserProjectComponent },
      { path: 'teamsmanager', component: TeamsManagerComponent },
      { path: 'teamsmanager/addteam', component: AddTeamComponent },
      { path: 'questions', component: QuestionsManagerComponent }
    ]
  },
  { path: 'evaluationsections', component: MenunewevaluationComponent },
  { path: 'evaluationquestions', component: NewevaluationComponent },
  { path: 'evaluationresults', component: PdfgeneratorComponent },
  { path: 'finishedevaluations', component: PreviousevaluationComponent },
  { path: 'pendingevaluations', component: PendingevaluationComponent },
  { path: '**', component: HomeComponent }

];

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  exports: [RouterModule]
})
export class AppRoutingModule { }

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);



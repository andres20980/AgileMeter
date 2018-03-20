import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import {ModuleWithProviders} from '@angular/core';

import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { NewevaluationComponent } from './newevaluation/newevaluation.component';

const appRoutes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'nuevaevaluacion', component: NewevaluationComponent },
  { path: '**', component: LoginComponent }
];

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  exports: [ RouterModule ]
})
export class AppRoutingModule { }

export const routing: ModuleWithProviders= RouterModule.forRoot(appRoutes);



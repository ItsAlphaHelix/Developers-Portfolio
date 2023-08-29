import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import HomeComponent from './components/home-page/home-page.component';
import { RegisterComponent } from './components/register-page/register-page.component';
import { LoginComponent } from './components/login-page/login-page.component';
import { ForgottenPasswordComponent } from './components/forgotten-password-page/forgotten-password-page.component';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { AboutComponent } from './components/about-page/about-me-page.component';
import { EditAboutPageComponent } from './components/edit-about-page/edit-about-page.component';
import { AddAboutInformationComponent } from './components/add-about-page/add-about-page.component';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegisterComponent,
    LoginComponent,
    ForgottenPasswordComponent,
    ErrorPageComponent,
    AboutComponent,
    AddAboutInformationComponent,
    EditAboutPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }


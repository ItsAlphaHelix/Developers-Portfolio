import { NgModule } from '@angular/core';
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
import { FooterComponent } from './components/footer-page/footer-page.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import ProjectComponent from './components/project-page/project-page.component';
import { AddProjectComponent } from './components/add-project-page/add-project-page.component';
import { ProjectDetailsComponent } from './components/project-details-page/project-details-page.component';
import { EditProjectPageComponent } from './components/edit-project-page/edit-project-page.component';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './services/authorization.interceptor';

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
    EditAboutPageComponent,
    FooterComponent,
    ProjectComponent,
    AddProjectComponent,
    ProjectDetailsComponent,
    EditProjectPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
    })
  ],
  providers: [
   {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
   }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

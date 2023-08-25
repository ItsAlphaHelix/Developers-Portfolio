import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest } from '../models/register-request-model';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { timeout } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientSideValidation {

  constructor(private toastr: ToastrService) { }

  registerFormValidation(registerForm?: FormGroup) {

    if (registerForm !== undefined) {

      if (registerForm.controls['email'].hasError('required') || registerForm.controls['username'].hasError('required') || registerForm.controls['firstName'].hasError('required') || registerForm.controls['lastName'].hasError('required') || registerForm.controls['password'].hasError('required') || registerForm.controls['confirmPassword'].hasError('required') || registerForm.controls['jobTitle'].hasError('required')) {

        this.toastr.error('All fields are required!');
        return;

      }

      if (registerForm.controls['email'].invalid && !(registerForm.controls['email'].hasError('required') && registerForm.controls['username'].hasError('required') && registerForm.controls['firstName'].hasError('required') && registerForm.controls['lastName'].hasError('required') && registerForm.controls['jobTitle'].hasError('required') && registerForm.controls['password'].hasError('required') && registerForm.controls['confirmPassword'].hasError('required'))) {
        this.toastr.error('Invalid email address!');
        return;
      }

      if (registerForm.controls['password'].hasError('pattern') && !(registerForm.controls['password'].hasError('required') && registerForm.controls['firstName'].hasError('required') && registerForm.controls['lastName'].hasError('required') && registerForm.controls['jobTitle'].hasError('required') && registerForm.controls['username'].hasError('required') && registerForm.controls['email'].hasError('required') && registerForm.controls['confirmPassword'].hasError('required'))) {

        this.toastr.error('Invalid password. Password should be at least 8 characters long and also should contain at least one lower case, one upper case, one digit and one special symbol.');
        return;

      }

      if (registerForm.controls['password'].invalid || registerForm.controls['confirmPassword'].invalid && !(registerForm.controls['email'].hasError('required') && registerForm.controls['username'].hasError('required') && registerForm.controls['firstName'].hasError('required') && registerForm.controls['lastName'].hasError('required') && registerForm.controls['jobTitle'].hasError('required') && registerForm.controls['password'].hasError('required') && registerForm.controls['confirmPassword'].hasError('required'))) {
        this.toastr.error('Password doesn\'t match!');
        return;
      }
    }
  }

  loginFormValidation(loginForm: FormGroup) {

    if (loginForm !== undefined) {

      if (loginForm.controls['email'].hasError('required') || loginForm.controls['password'].hasError('required')) {

        this.toastr.error('All fields are required');
        return;

      }
      if (loginForm.controls['email'].invalid && !loginForm.controls['email'].hasError('required')) {

        this.toastr.error('Invalid email address!');
        return;
      }
    }
  }

  aboutUserFormValidation(aboutForm: FormGroup) {
    if (aboutForm.controls['phoneNumber'].hasError('required') || aboutForm.controls['city'].hasError('required') || aboutForm.controls['country'].hasError('required') || aboutForm.controls['age'].hasError('required') || aboutForm.controls['education'].hasError('required') || aboutForm.controls['aboutDescription'].hasError('required')) {

      this.toastr.error('All fields are required!');
      return;

    }
  }
}
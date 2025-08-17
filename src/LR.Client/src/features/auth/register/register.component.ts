import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { TextInputComponent } from "../../../shared/text-input/text-input.component";

@Component({
  selector: 'app-register',
  imports: [
    TextInputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);

  protected registerForm: FormGroup;

  public registerImage = ASSETS.IMAGES.ILLUSTRATIONS.REGISTER;


  constructor() {
    this.registerForm = this.fb.group({
      userName: ['', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(20)
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(20),
        this.containDigit,
        this.containLowercase,
        this.containUppercase
      ]],
      confirmPassword: ['', [Validators.required]],
      firstName: ['', [Validators.maxLength(50)]],
      lastName: ['', [Validators.maxLength(50)]],
      email: ['', [Validators.email]]
    }, { validators: this.passwordsMatch });
  }

  private containDigit(control: AbstractControl): ValidationErrors | null {
    return /\d/.test(control.value) ? null : { digit: true };
  }

  private containLowercase(control: AbstractControl): ValidationErrors | null {
    return /[a-z]/.test(control.value) ? null : { lowercase: true };
  }

  private containUppercase(control: AbstractControl): ValidationErrors | null {
    return /[A-Z]/.test(control.value) ? null : { uppercase: true };
  }

  private passwordsMatch(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value;
  const confirmPassword = group.get('confirmPassword')?.value;
  
  return password === confirmPassword ? null : { passwordMismatch: true };
}

  public register() {}
}

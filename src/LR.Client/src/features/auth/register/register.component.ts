import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { TextInputComponent } from "../../../shared/text-input/text-input.component";
import { ValidationIcon, ValidationIndicator } from '../../../core/types/utils/validation.type';

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
  private USERNAME_MIN_LENGTH: number = 4;
  private USERNAME_MAX_LENGTH: number = 20;
  private PASSWORD_MIN_LENGTH: number = 8;
  private PASSWORD_MAX_LENGTH: number = 20;
  private NAME_MAX_LENGTH: number = 50;

  protected registerForm: FormGroup;

  public registerImage = ASSETS.IMAGES.ILLUSTRATIONS.REGISTER;
  public validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;
  public userNameValidationIndicators: ValidationIndicator[];
  public passwordValidationIndicators: ValidationIndicator[];


  constructor() {

    this.registerForm = this.fb.group({
      userName: ['', [
        Validators.required,
        Validators.minLength(this.USERNAME_MIN_LENGTH),
        Validators.maxLength(this.USERNAME_MAX_LENGTH)
      ]],
      password: ['', [
        Validators.required,
        Validators.maxLength(this.PASSWORD_MAX_LENGTH),
        this.passwordValidator
      ]],
      confirmPassword: ['', [
        Validators.required,
        this.matchValues('password')
      ]],
      firstName: ['', [Validators.maxLength(this.NAME_MAX_LENGTH)]],
      lastName: ['', [Validators.maxLength(this.NAME_MAX_LENGTH)]],
      email: ['', [Validators.email]]
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });

    this.userNameValidationIndicators = [
      this.createIndicator('required', 'Password is required', this.validationIcons.REQUIRED),
      this.createIndicator('minlength', `Must be at least ${this.USERNAME_MIN_LENGTH} characters`, this.validationIcons.MIN),
      this.createIndicator('maxlength', `Must be at most ${this.USERNAME_MAX_LENGTH} characters`, this.validationIcons.MAX),
    ];

    this.passwordValidationIndicators = [
      this.createIndicator('required', 'Password is required', this.validationIcons.REQUIRED),
      this.createIndicator('lowercase', 'Must contain at least one lowercase letter', this.validationIcons.L_CASE),
      this.createIndicator('uppercase', 'Must contain at least one uppercase letter', this.validationIcons.U_CASE),
      this.createIndicator('digit', 'Must contain at least one digit', this.validationIcons.DIGIT),
      this.createIndicator('minlength', `Must be at least ${this.PASSWORD_MIN_LENGTH} characters`, this.validationIcons.MIN),
      this.createIndicator('maxlength', `Must be at most ${this.PASSWORD_MAX_LENGTH} characters`, this.validationIcons.MAX),
    ];
  }

  private passwordValidator = (control: AbstractControl): ValidationErrors | null => {
    const value = control.value ?? '';
    const errors: ValidationErrors = {};

    if (value.length < this.PASSWORD_MIN_LENGTH) {
      errors['minlength'] = { requiredLength: this.PASSWORD_MIN_LENGTH, actualLength: value.length };
    }
    if (!/\d/.test(value)) {
      errors['digit'] = true;
    }
    if (!/[a-z]/.test(value)) {
      errors['lowercase'] = true;
    }
    if (!/[A-Z]/.test(value)) {
      errors['uppercase'] = true;
    }

    return Object.keys(errors).length ? errors : null;
  };

  private matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { isMatching: true }
    }
  };

  private createIndicator(key: string, message: string, icons: ValidationIcon): ValidationIndicator {
    return {
      key,
      message,
      icons
    };
  };

  public register() { };
}

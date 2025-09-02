import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../../core/constants/assets.constants';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { TextInputComponent } from "../../../../shared/components/text-input/text-input.component";
import { ValidationIndicator } from '../../../../core/types/utils/validation.type';
import { USER_AUTH } from '../../../../core/constants/validation.constants';
import { digitValidator, lowercaseValidator, minLengthInstant, uppercaseValidator } from '../../../../shared/validators/string-pattern.validator';
import { ValidationIndicatorService } from '../../../../core/services/utils/validation-indicator.service';

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
  private validationIndicatorService = inject(ValidationIndicatorService);

  protected registerForm: FormGroup = new FormGroup({});

  public registerImage = ASSETS.IMAGES.ILLUSTRATIONS.REGISTER;
  public validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;
  public validationIndicators: Record<string, ValidationIndicator[]> = {};

  constructor() {
    this.initForm();
    this.initIndicators();
  }

  private initForm(): void {
    this.registerForm = this.fb.group({
      userName: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.USERNAME_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.USERNAME_MAX_LENGTH)
      ]],
      password: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.PASSWORD_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.PASSWORD_MAX_LENGTH),
        digitValidator,
        lowercaseValidator,
        uppercaseValidator
      ]],
      confirmPassword: ['', [
        Validators.required,
        this.matchValues('password')
      ]],
      firstName: ['', [Validators.maxLength(USER_AUTH.NAME_MAX_LENGTH)]],
      lastName: ['', [Validators.maxLength(USER_AUTH.NAME_MAX_LENGTH)]],
      email: ['', [Validators.email]]
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  private initIndicators() {
    this.validationIndicators = {
      userName: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Username' },
        { indicator: 'minlength', param: USER_AUTH.USERNAME_MIN_LENGTH },
        { indicator: 'maxlength', param: USER_AUTH.USERNAME_MAX_LENGTH }
      ]),
      password: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Password' }, 
        { indicator: 'minlength', param: USER_AUTH.PASSWORD_MIN_LENGTH },
        { indicator: 'maxlength', param: USER_AUTH.PASSWORD_MAX_LENGTH },
        { indicator: 'lowercase' },
        { indicator: 'uppercase' },
        { indicator: 'digit' }
      ]),
      confirmPassword: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Password confirmation' }, 
        { indicator: 'passwordMismatching' }
      ]),
      firstName: this.validationIndicatorService.getIndicators([
        { indicator: 'maxlength', param: USER_AUTH.NAME_MAX_LENGTH }
      ]),
      lastName:  this.validationIndicatorService.getIndicators([
        { indicator: 'maxlength', param: USER_AUTH.NAME_MAX_LENGTH }
      ]),
      email: this.validationIndicatorService.getIndicators([
        { indicator: 'email' }
      ]),
    };
  }

  private matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { passwordMismatching: true }
    }
  };

  public register() { };
}

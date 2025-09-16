import { Component, inject } from '@angular/core';
import { AuthStore } from '../../../../auth/store/auth.store';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ValidationIndicatorService } from '../../../../../core/services/utils/validation-indicator.service';
import { ASSETS } from '../../../../../core/constants/assets.constants';
import { ValidationIndicator } from '../../../../../core/types/utils/validation.type';
import { digitValidator, lowercaseValidator, matchToFieldValue, minLengthInstant, uppercaseValidator } from '../../../../../shared/validators/string-pattern.validator';
import { USER_AUTH } from '../../../../../core/constants/validation.constants';
import { TextInputComponent } from "../../../../../shared/components/text-input/text-input.component";
import { BusyComponent } from "../../../../../shared/components/busy/busy.component";

@Component({
  selector: 'app-change-password',
  imports: [
    TextInputComponent,
    ReactiveFormsModule,
    BusyComponent
],
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent {
  private fb = inject(FormBuilder);
  private validationIndicatorService = inject(ValidationIndicatorService);

  protected changePassForm: FormGroup = new FormGroup({});

  public authStore = inject(AuthStore);
  public validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;
  public validationIndicators: Record<string, ValidationIndicator[]> = {};

  constructor() {
    this.initForm();
    this.initIndicators();
  }

  private initForm(): void {
    this.changePassForm = this.fb.group({
      currentPassword: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.PASSWORD_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.PASSWORD_MAX_LENGTH),
      ]],
      newPassword: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.PASSWORD_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.PASSWORD_MAX_LENGTH),
        digitValidator,
        lowercaseValidator,
        uppercaseValidator
      ]],
      confirmPassword: ['', [
        Validators.required,
        matchToFieldValue('password')
      ]],
    });

    this.changePassForm.controls['newPassword'].valueChanges.subscribe({
      next: () => this.changePassForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }

  private initIndicators(): void {
    this.validationIndicators = {
      currentPassword: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Password' },
        { indicator: 'minlength', param: USER_AUTH.PASSWORD_MIN_LENGTH },
        { indicator: 'maxlength', param: USER_AUTH.PASSWORD_MAX_LENGTH },
      ]),
      newPassword: this.validationIndicatorService.getIndicators([
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
    };
  }

  public changePassword(): void {
    
  }
}

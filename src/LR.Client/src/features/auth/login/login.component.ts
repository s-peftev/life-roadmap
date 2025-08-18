import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationIndicator } from '../../../core/types/utils/validation.type';
import { USER_AUTH } from '../../../core/constants/validation.constants';
import { minLengthInstant } from '../../../shared/validators/string-pattern.validator';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);

  protected loginForm: FormGroup = new FormGroup({});

  public loginImage = ASSETS.IMAGES.ILLUSTRATIONS.LOGIN;
  public validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;
  public validationIndicators: Record<string, ValidationIndicator[]> = {};

  constructor() {
    this.initForm();
  }

  private initForm(): void {
    this.loginForm = this.fb.group({
      userName: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.USERNAME_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.USERNAME_MAX_LENGTH)
      ]],
      password: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.PASSWORD_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.PASSWORD_MAX_LENGTH),
      ]],
    });
  }
}

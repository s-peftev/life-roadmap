import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../../core/constants/assets.constants';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ValidationIndicator } from '../../../../core/types/utils/validation.type';
import { USER_AUTH } from '../../../../core/constants/validation.constants';
import { minLengthInstant } from '../../../../shared/validators/string-pattern.validator';
import { TextInputComponent } from '../../../../shared/components/text-input/text-input.component';
import { ValidationIndicatorService } from '../../../../core/services/utils/validation-indicator.service';
import { AuthStore } from '../../store/auth.store';
import { BusyComponent } from "../../../../shared/components/busy/busy.component";
import { LoginRequest } from '../../../../models/auth/login-request.model';

@Component({
  selector: 'app-login',
  imports: [
    TextInputComponent,
    ReactiveFormsModule,
    BusyComponent
],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private validationIndicatorService = inject(ValidationIndicatorService);

  protected loginForm: FormGroup = new FormGroup({});

  public loginImage = ASSETS.IMAGES.ILLUSTRATIONS.LOGIN;
  public validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;
  public validationIndicators: Record<string, ValidationIndicator[]> = {};
  public authStore = inject(AuthStore);

  constructor() {
    this.initForm();
    this.initIndicators();
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
      ])
    };
  }

  public login() {
    if (this.loginForm.invalid) return;

    const request: LoginRequest = this.loginForm.value;

    this.authStore.login(request);
  }
}

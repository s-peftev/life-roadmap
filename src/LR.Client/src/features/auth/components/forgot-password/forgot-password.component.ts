import { Component, inject, OnInit } from '@angular/core';
import { AuthStore } from '../../store/auth.store';
import { BusyComponent } from "../../../../shared/components/busy/busy.component";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ValidationIndicatorService } from '../../../../core/services/utils/validation-indicator.service';
import { ValidationIndicator } from '../../../../core/types/utils/validation.type';
import { TextInputComponent } from '../../../../shared/components/text-input/text-input.component';
import { minLengthInstant } from '../../../../shared/validators/string-pattern.validator';
import { USER_AUTH } from '../../../../core/constants/validation.constants';
import { RouterLink } from '@angular/router';
import { ROUTES } from '../../../../core/constants/routes.constants';
import { ForgotPasswordRequest } from '../../../../models/auth/forgot-password-request.model';

@Component({
  selector: 'app-forgot-password',
  imports: [
    BusyComponent,
    ReactiveFormsModule,
    TextInputComponent,
    RouterLink
  ],
  templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent implements OnInit{
  private fb = inject(FormBuilder);
  private validationIndicatorService = inject(ValidationIndicatorService);
  
  protected forgotPasswordForm: FormGroup = new FormGroup({});

  public authStore = inject(AuthStore);
  public validationIndicators: Record<string, ValidationIndicator[]> = {};
  public ROUTES = ROUTES.AUTH;

  constructor() {
    this.initForm();
    this.initIndicators();
  }

  ngOnInit(): void {
    this.authStore.setPasswordResetRequested(false);
  }

  private initForm(): void {
    this.forgotPasswordForm = this.fb.group({
      userName: ['', [
        Validators.required,
        minLengthInstant(USER_AUTH.USERNAME_MIN_LENGTH),
        Validators.maxLength(USER_AUTH.USERNAME_MAX_LENGTH)
      ]],
      email: ['', [
        Validators.required,
        Validators.email
      ]]
    });
  }

  private initIndicators(): void {
    this.validationIndicators = {
      userName: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Username'},
        { indicator: 'minlength', param: USER_AUTH.USERNAME_MIN_LENGTH },
        { indicator: 'maxlength', param: USER_AUTH.USERNAME_MAX_LENGTH }
      ]),
      email: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Email'},
        { indicator: 'email' }
      ])
    }
  }

  public resetPasswordRequest() {
    if (this.forgotPasswordForm.invalid) return;

    const request: ForgotPasswordRequest = this.forgotPasswordForm.value;

    this.authStore.resetPasswordRequest(request)
  }
}

import { Component, inject, OnInit } from '@angular/core';
import { BusyComponent } from "../../../../shared/components/busy/busy.component";
import { AuthStore } from '../../store/auth.store';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ValidationIndicatorService } from '../../../../core/services/utils/validation-indicator.service';
import { TextInputComponent } from '../../../../shared/components/text-input/text-input.component';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ValidationIndicator } from '../../../../core/types/utils/validation.type';
import { ROUTES } from '../../../../core/constants/routes.constants';
import { digitValidator, lowercaseValidator, matchToFieldValue, minLengthInstant, uppercaseValidator } from '../../../../shared/validators/string-pattern.validator';
import { USER_AUTH } from '../../../../core/constants/validation.constants';
import { ResetPasswordRequest } from '../../../../models/auth/reset-password-request.model';

@Component({
  selector: 'app-reset-password',
  imports: [
    BusyComponent,
    ReactiveFormsModule,
    TextInputComponent,
    RouterLink
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  private fb = inject(FormBuilder);
  private validationIndicatorService = inject(ValidationIndicatorService);
  private route = inject(ActivatedRoute);
  private userId: string | null = null;
  private token: string | null = null;

  protected resetPasswordForm: FormGroup = new FormGroup({});

  public authStore = inject(AuthStore);
  public validationIndicators: Record<string, ValidationIndicator[]> = {};
  public ROUTES = ROUTES.AUTH;

  constructor() {
    this.initForm();
    this.initIndicators();
  }

  ngOnInit() {
    const userIdKey: keyof ResetPasswordRequest = 'userId';
    const tokenKey: keyof ResetPasswordRequest = 'token';

    this.userId = this.route.snapshot.queryParamMap.get(userIdKey);
    this.token = this.route.snapshot.queryParamMap.get(tokenKey);
  }

  private initForm(): void {
    this.resetPasswordForm = this.fb.group({
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
        matchToFieldValue('password')
      ]],
    });
  }

  private initIndicators(): void {
    this.validationIndicators = {
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
      ])
    };
  }

  public reset() {
    if (this.resetPasswordForm.invalid) return;

    const request: ResetPasswordRequest = {
      ...this.resetPasswordForm.value,
      token: this.token,
      userId: this.userId
    }

    this.authStore.resetPassword(request);
  }
}

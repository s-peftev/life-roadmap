import { inject, Injectable } from '@angular/core';
import { ValidationIcon, ValidationIndicator } from '../../types/utils/validation.type';
import { ASSETS } from '../../constants/assets.constants';
import { TranslateService } from '@ngx-translate/core';

type IndicatorParameter = {
  indicator: string;
  param?: number | string;
}

@Injectable({
  providedIn: 'root'
})
export class ValidationIndicatorService {
  private translateServise = inject(TranslateService);
  private validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;

  private createIndicator = (key: string, messageKey: string, icons: ValidationIcon, params?: Record<string, any>): ValidationIndicator =>
    ({ 
      key,
      message$: this.translateServise.stream(messageKey, params),
      icons 
    });

  private validationIndicators: Record<string, (param?: number | string) => ValidationIndicator> = {
    required: () => this.createIndicator('required', 'validation_indicator.required', this.validationIcons.REQUIRED),
    minlength: (param) => this.createIndicator('minlength', 'validation_indicator.minlength', this.validationIcons.MIN, { length: param }),
    maxlength: (param) => this.createIndicator('maxlength', 'validation_indicator.maxlength', this.validationIcons.MAX, { length: param }),
    lowercase: () => this.createIndicator('lowercase', 'validation_indicator.lowercase', this.validationIcons.L_CASE),
    uppercase: () => this.createIndicator('uppercase', 'validation_indicator.uppercase', this.validationIcons.U_CASE),
    digit: () => this.createIndicator('digit', 'validation_indicator.digit', this.validationIcons.DIGIT),
    passwordMismatching: () => this.createIndicator('passwordMismatching', 'validation_indicator.passwordMismatching', this.validationIcons.PASS_MATCH),
    email: () => this.createIndicator('email', 'validation_indicator.email', this.validationIcons.EMAIL)
  }

  public getIndicators(indicatorParams: IndicatorParameter[]): ValidationIndicator[] {
    return indicatorParams.flatMap(indicatorParameter => 
      this.validationIndicators[indicatorParameter.indicator]?.(indicatorParameter.param) ?? []);
  }
}

import { Injectable } from '@angular/core';
import { ValidationIcon, ValidationIndicator } from '../../types/utils/validation.type';
import { ASSETS } from '../../constants/assets.constants';

type IndicatorParameter = {
  indicator: string;
  param?: number | string;
}

@Injectable({
  providedIn: 'root'
})
export class ValidationIndicatorService {

  private validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;

  private createIndicator = (key: string, message: string, icons: ValidationIcon): ValidationIndicator =>
    ({ key, message, icons });

  private validationIndicators: Record<string, (param?: number | string) => ValidationIndicator> = {
    required: (param) => this.createIndicator('required', `${param} is required`, this.validationIcons.REQUIRED),
    minlength: (param) => this.createIndicator('minlength', `Must be at least ${param} characters`, this.validationIcons.MIN),
    maxlength: (param) => this.createIndicator('maxlength', `Must be at most ${param} characters`, this.validationIcons.MAX),
    lowercase: () => this.createIndicator('lowercase', 'Must contain at least one lowercase letter', this.validationIcons.L_CASE),
    uppercase: () => this.createIndicator('uppercase', 'Must contain at least one uppercase letter', this.validationIcons.U_CASE),
    digit: () => this.createIndicator('digit', 'Must contain at least one digit', this.validationIcons.DIGIT),
    passwordMismatching: () => this.createIndicator('passwordMismatching', 'Passwords must match', this.validationIcons.PASS_MATCH),
    email: () => this.createIndicator('email', `Email must have a valid format`, this.validationIcons.EMAIL)
  }

  public getIndicators(indicatorParams: IndicatorParameter[]): ValidationIndicator[] {
    return indicatorParams.flatMap(indicatorParameter => 
      this.validationIndicators[indicatorParameter.indicator]?.(indicatorParameter.param) ?? []);
  }
}

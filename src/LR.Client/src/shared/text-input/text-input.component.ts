import { NgFor, NgIf, } from '@angular/common';
import { Component, input, Self, signal } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule, ValidationErrors } from '@angular/forms';
import { ValidationIndicator } from '../../core/types/utils/validation.type';

@Component({
  selector: 'app-text-input',
  imports: [NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.css'
})
export class TextInputComponent implements ControlValueAccessor {
  public label = input<string>('');
  public type = input<'text' | 'password' | 'email' | 'date'>('text');
  public indicators = input<ValidationIndicator[]>([]);
  public touchedOnce = signal(false);

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void { };
  registerOnChange(fn: any): void { };
  registerOnTouched(fn: any): void { };
  setDisabledState?(isDisabled: boolean): void { };

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

  getIcon(indicator: ValidationIndicator): string {
    if (!indicator.icons) return '';
    return this.control.hasError(indicator.key) ? indicator.icons.ERR : indicator.icons.OK;
  }
}

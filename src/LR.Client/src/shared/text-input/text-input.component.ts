import { NgFor, NgIf } from '@angular/common';
import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  imports: [NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.css'
})
export class TextInputComponent implements ControlValueAccessor{
  public label = input<string>('');
  public type = input<'text' | 'password' | 'email' | 'date'>('text');

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}
  setDisabledState?(isDisabled: boolean): void {}

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

  get formGroupErrors(): ValidationErrors | null {
    return this.control.parent?.errors || null;
  }

  get errorMessages(): string[] {
    if (!this.control || !this.control.errors) return [];
    const errors = this.control.errors;
    const messages: string[] = [];

    if(this.control.touched) {
      if (errors['required']) messages.push(`Please enter a ${this.label()}`);
      if (errors['minlength'])
        messages.push(`${this.label()} must be at least ${errors['minlength'].requiredLength} characters.`);
      if (errors['maxlength'])
        messages.push(`${this.label()} must be at most ${errors['maxlength'].requiredLength} characters.`);
      if (errors['digit']) messages.push(`${this.label()} must contain at least one digit.`);
      if (errors['lowercase']) messages.push(`${this.label()} must contain at least one lowercase letter.`);
      if (errors['uppercase']) messages.push(`${this.label()} must contain at least one uppercase letter.`);
      if (errors['passwordMismatch']) messages.push(`Passwords do not match.`);
    }

    return messages;
  }
}

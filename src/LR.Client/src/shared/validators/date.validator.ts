import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function birthDateValidator(maxAge: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) return null;

    const value = new Date(control.value);
    const today = new Date();
    const minDate = new Date();
    minDate.setFullYear(today.getFullYear() - maxAge);

    if (value > today) {
      return { futureDate: 'BirthDate cannot be in the future.' };
    }

    if (value <= minDate) {
      return { tooOld: 'BirthDate is not valid.' };
    }

    return null;
  };
}
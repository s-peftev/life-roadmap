import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function digitValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value ?? '';
    return /\d/.test(value) ? null : { digit: true };
}

export function lowercaseValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value ?? '';
    return /[a-z]/.test(value) ? null : { lowercase: true };
}

export function uppercaseValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value ?? '';
    return /[A-Z]/.test(value) ? null : { uppercase: true };
}

//displays error even when field is empty
export function minLengthInstant(minLength: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value ?? '';
        const errors: ValidationErrors = {};

        if (value.length < minLength) {
            errors['minlength'] = { requiredLength: minLength, actualLength: value.length };
        }

        return Object.keys(errors).length ? errors : null;
    }
}
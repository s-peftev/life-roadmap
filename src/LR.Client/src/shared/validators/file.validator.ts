import { AbstractControl, ValidatorFn } from "@angular/forms";

export function fileSizeValidator(maxSize: number, message?: string): ValidatorFn {
  return (control: AbstractControl) => {
    const file = control.value as File;
    if (file && file.size > maxSize) {
      return { fileTooLarge: message ?? `File is too large (max ${maxSize / 1024 / 1024}MB)` };
    }
    return null;
  };
}

export function fileTypeValidator(allowedTypes: string[], message?: string): ValidatorFn {
  return (control: AbstractControl) => {
    const file = control.value as File;
    if (file && !allowedTypes.includes(file.type)) {
      return { invalidFileType: message ?? `Allowed types: ${allowedTypes.join(', ')}` };
    }
    return null;
  };
}
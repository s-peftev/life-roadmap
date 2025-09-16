import { Component, inject, signal } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { ROUTES } from '../../../core/constants/routes.constants';
import { ProfileStore } from './store/profile.store';
import { BusyComponent } from "../../../shared/components/busy/busy.component";
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { fileSizeValidator, fileTypeValidator } from '../../../shared/validators/file.validator';
import { KeyValuePipe, NgFor } from '@angular/common';
import { TextInputComponent } from "../../../shared/components/text-input/text-input.component";
import { minLengthInstant } from '../../../shared/validators/string-pattern.validator';
import { USER_AUTH, USER_PROFILE } from '../../../core/constants/validation.constants';
import { ValidationIndicator } from '../../../core/types/utils/validation.type';
import { ValidationIndicatorService } from '../../../core/services/utils/validation-indicator.service';
import { ChangeUserNameRequest } from '../../../models/user-profile/change-username-request';
import { birthDateValidator } from '../../../shared/validators/date.validator';
import { ChangePersonalInfoRequest } from '../../../models/user-profile/change-personal-info-request';

@Component({
  selector: 'app-profile-settings',
  imports: [
    BusyComponent,
    NgFor,
    KeyValuePipe,
    TextInputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './profile-settings.component.html'
})
export class ProfileSettingsComponent {
  private fb = inject(FormBuilder);
  private validationIndicatorService = inject(ValidationIndicatorService);

  protected personalForm: FormGroup = new FormGroup({});

  public usernameEditMode = signal<boolean>(false);
  public personalEditMode = signal<boolean>(false);
  public profileStore = inject(ProfileStore);
  public routes = ROUTES;
  public icons = ASSETS.IMAGES.ICONS
  public validationIcons = ASSETS.IMAGES.ICONS.VALIDATION;
  public validationIndicators: Record<string, ValidationIndicator[]> = {};

  fileControl = new FormControl<File | null>(null, [
    fileSizeValidator(4 * 1024 * 1024, 'That photo is a bit too large. Try something smaller.'),
    fileTypeValidator(['image/png', 'image/jpeg'])
  ]);

  usernameControl = new FormControl<string>('', [
    Validators.required,
    minLengthInstant(USER_AUTH.USERNAME_MIN_LENGTH),
    Validators.maxLength(USER_AUTH.USERNAME_MAX_LENGTH)
  ])

  constructor() {
    this.usernameControl.setValue(this.profileStore.userName());
    this.initIndicators();
    this.initForm();
  }

  private initForm(): void {
    this.personalForm = this.fb.group({
      firstName: ['', [
        Validators.maxLength(USER_AUTH.NAME_MAX_LENGTH)
      ]],
      lastName: ['', [
        Validators.maxLength(USER_AUTH.NAME_MAX_LENGTH),
      ]],
      birthDate: ['', [
        birthDateValidator(USER_PROFILE.USER_MAX_AGE)
      ]],
    });

    this.personalForm.controls['firstName'].setValue(this.profileStore.firstName());
    this.personalForm.controls['lastName'].setValue(this.profileStore.lastName());
    this.personalForm.controls['birthDate'].setValue(this.profileStore.birthDate());
  }

  private initIndicators(): void {
    this.validationIndicators = {
      userName: this.validationIndicatorService.getIndicators([
        { indicator: 'required', param: 'Username' },
        { indicator: 'minlength', param: USER_AUTH.USERNAME_MIN_LENGTH },
        { indicator: 'maxlength', param: USER_AUTH.USERNAME_MAX_LENGTH }
      ]),
      firstName: this.validationIndicatorService.getIndicators([
        { indicator: 'maxlength', param: USER_AUTH.NAME_MAX_LENGTH }
      ]),
      lastName: this.validationIndicatorService.getIndicators([
        { indicator: 'maxlength', param: USER_AUTH.NAME_MAX_LENGTH }
      ]),
    };
  }

  public toggleUsernameEdit() {
    this.usernameEditMode.set(!this.usernameEditMode());
  }

  public togglePersonalEdit() {
    this.personalEditMode.set(!this.personalEditMode());
  }

  public uploadProfilePhoto(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file: File = input.files[0];
    this.fileControl.setValue(file);

    if (this.fileControl.valid)
      this.profileStore.uploadProfilePhoto(file);
  }

  public deleteProfilePhoto() {
    this.profileStore.deleteProfilePhoto();
  }

  public changeUserName() {
    if (!this.usernameControl.valid) return;

    if (this.profileStore.userName() === this.usernameControl.value) {
      this.toggleUsernameEdit();
      return;
    }

    const request: ChangeUserNameRequest = { userName: this.usernameControl.value! };

    this.profileStore.changeUserName(request).subscribe({
      next: () => this.toggleUsernameEdit(),
      error: () => { }
    });
  }

  public changePersonalInfo() {
    if (!this.personalForm.valid) return;

    if (
      this.profileStore.firstName() === this.personalForm.controls['firstName'].value &&
      this.profileStore.lastName() === this.personalForm.controls['lastName'].value &&
      this.profileStore.birthDate() === this.personalForm.controls['birthDate'].value
    ) {
      this.togglePersonalEdit();
      return;
    }

    const request: ChangePersonalInfoRequest = this.personalForm.value;

    this.profileStore.changePersonalInfo(request).subscribe({
      next: () => this.togglePersonalEdit(),
      error: () => { }
    });
  }
}

import { Component, input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ValidationIndicator } from '../../../../../core/types/utils/validation.type';
import { TextInputComponent } from '../../../../../shared/components/text-input/text-input.component';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-profile-field',
  imports: [
    ReactiveFormsModule,
    TextInputComponent,
    TranslatePipe
  ],
  templateUrl: './profile-field.component.html',
})
export class ProfileFieldComponent {
  public label = input.required<string>();
  public value = input<string | null>(null);
  public editMode = input.required<boolean>();
  public control = input.required<FormControl>();
  public indicators = input<ValidationIndicator[] | null>(null)
  public type = input<'text' | 'date'> ('text');
}

import { KeyValuePipe, NgFor, NgIf } from '@angular/common';
import { Component, input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-base-validation-messages',
  imports: [
    NgFor,
    NgIf,
    KeyValuePipe
  ],
  templateUrl: './base-validation-messages.component.html',
})
export class BaseValidationMessagesComponent {
  public control = input.required<FormControl>()
}

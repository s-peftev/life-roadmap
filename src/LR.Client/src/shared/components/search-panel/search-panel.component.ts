import { Component, computed, input, OnInit, output } from '@angular/core';
import { SearchFieldOption } from '../../../core/interfaces/search-field-option.interface';
import { TranslatePipe } from '@ngx-translate/core';
import { TextSearchable } from '../../../core/interfaces/text-searchable.interface';

@Component({
  selector: 'app-search-panel',
  imports: [
    TranslatePipe
  ],
  templateUrl: './search-panel.component.html'
})
export class SearchPanelComponent<T> implements OnInit {
  public fields = input.required<SearchFieldOption<T>[]>()
  public initialFields = input.required<T[]>();
  public searchRequest = output<TextSearchable<T>>();

  public selectedFields: T[] = [];

  ngOnInit() {
    this.selectedFields = this.initialFields();
  }

  public onToggle(key: T, inputValue: string) {
    this.selectedFields = this.selectedFields.includes(key) 
      ? this.selectedFields.filter(x => x !== key) 
      : [...this.selectedFields, key];

    this.searchRequest.emit(this.buildSearchRequest(inputValue, this.selectedFields));
  }

  public onInput(inputValue: string) {
    this.searchRequest.emit(this.buildSearchRequest(inputValue, this.selectedFields));
  }

  private buildSearchRequest(searchText: string, fields: T[]): TextSearchable<T> {
    return {
      searchText,
      fields
    }
  }
}

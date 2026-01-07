import { Component, input, OnInit, output } from '@angular/core';
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
export class SearchPanelComponent<T> {
  public fields = input.required<SearchFieldOption<T>[]>()
  public selectedFields = input.required<Set<T>>();
  public searchRequest = output<TextSearchable<T>>();

  public onToggle(key: T, inputValue: string) {
    this.selectedFields().has(key)
      ? this.selectedFields().delete(key)
      : this.selectedFields().add(key);

    this.searchRequest.emit(this.buildSearchRequest(inputValue, this.selectedFields()));
  }

  public onInput(inputValue: string) {
    this.searchRequest.emit(this.buildSearchRequest(inputValue, this.selectedFields()));
  }

  private buildSearchRequest(searchText: string, fields: Set<T>): TextSearchable<T> {
    return {
      searchText,
      fields
    }
  }
}

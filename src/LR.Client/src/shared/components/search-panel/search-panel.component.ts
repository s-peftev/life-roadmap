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
export class SearchPanelComponent<T> implements OnInit {
  public fields = input.required<SearchFieldOption<T>[]>()
  public searchRequest = output<TextSearchable>();

  public selected = new Set<T>();
  
  ngOnInit() {
    this.selectAll();
  }

  public onToggle(key: T, inputValue: string) {
    this.selected.has(key)
      ? this.selected.delete(key)
      : this.selected.add(key);

    this.searchRequest.emit(this.buildRequest(inputValue, this.selected));
  }

  public onInput(inputValue: string) {
    this.searchRequest.emit(this.buildRequest(inputValue, this.selected));
  }

  private buildRequest(searchText: string, fields: Set<T>): TextSearchable {
    return {
      searchText,
      fields
    }
  }

  private selectAll() {
    this.selected = new Set(this.fields().map(f => f.key));
  }
}

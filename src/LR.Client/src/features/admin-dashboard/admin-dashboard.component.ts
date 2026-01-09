import { DatePipe, NgFor } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { AdminStore } from './store/admin.store';
import { BusyComponent } from "../../shared/components/busy/busy.component";
import { ASSETS } from '../../core/constants/assets.constants';
import { UserForAdmin } from '../../models/admin/user-for-admin.model';
import { Role } from '../../core/enums/role.enum';
import { TranslatePipe } from '@ngx-translate/core';
import { PaginationComponent } from "../../shared/components/pagination/pagination.component";
import { SearchPanelComponent } from "../../shared/components/search-panel/search-panel.component";
import { UserSearchField } from '../../core/enums/search-fields/user-search-field.enum';
import { SearchFieldOption } from '../../core/interfaces/search-field-option.interface';
import { TextSearchable } from '../../core/interfaces/text-searchable.interface';
import { UserSortField } from '../../core/enums/sort/user-sort-field.enum';
import { SortDescriptor } from '../../core/interfaces/sort-descriptor.interface';
import { SortOrder } from '../../core/types/utils/sort-order.type';
import { MultiSortIndicatorComponent } from "../../shared/components/multi-sort-indicator/multi-sort-indicator.component";


@Component({
  selector: 'app-admin-dashboard',
  imports: [
    NgFor,
    BusyComponent,
    DatePipe,
    TranslatePipe,
    PaginationComponent,
    SearchPanelComponent,
    MultiSortIndicatorComponent
],
  templateUrl: './admin-dashboard.component.html',
})
export class AdminDashboardComponent {
  public adminStore = inject(AdminStore);
  public isBusy = computed(() => this.adminStore.isBusy());
  public icons = ASSETS.IMAGES.ICONS

  public sortCriteria: SortDescriptor<UserSortField>[] = [];

  public readonly sortFields = UserSortField;

  public searchFields: SearchFieldOption<UserSearchField>[] = [
    { key: UserSearchField.UserName, label: 'Username' },
    { key: UserSearchField.Email, label: 'Email' },
    { key: UserSearchField.FirstName, label: 'First Name' },
    { key: UserSearchField.LastName, label: 'Last Name' },
  ]

  public getRolesString(user: UserForAdmin): string {
    return user.roles.map(role => Role[role]).join(', ');
  }

  public deleteUserPhoto(userId: string): void {
    this.adminStore.deleteUserPhoto(userId);
  }

  public onSearchTextChange(searchRequest: TextSearchable<UserSearchField>): void {
    this.adminStore.setSearch(searchRequest);
  }

  public onChangePage(pageNumber: number): void {
    this.adminStore.setCurrentPage(pageNumber);
  }

  onSortToggle(field: UserSortField, multi: boolean = false) {
        const index = this.sortCriteria.findIndex(s => s.field === field);
        let currentOrder: SortOrder = null;

        if (index !== -1) {
            currentOrder = this.sortCriteria[index].order;
        }

        const nextOrder: SortOrder = currentOrder === 'asc' ? 'desc' : (currentOrder === 'desc' ? null : 'asc');

        if (!multi) {
            this.sortCriteria = [];
        }

        if (nextOrder) {
            const newSort = { field, order: nextOrder };
            if (index !== -1 && multi) {
                this.sortCriteria[index] = newSort;
            } else {
                this.sortCriteria.push(newSort);
            }
        } else if (index !== -1) {
            this.sortCriteria.splice(index, 1);
        }

        console.log(this.sortCriteria);

        this.adminStore.setSortCriteria(this.sortCriteria);
    }

    getSortOrder(field: string): SortOrder {
        return this.sortCriteria.find(s => s.field === field)?.order || null;
    }

    getSortIndex(field: UserSortField): number {
      const index = this.sortCriteria.findIndex(s => s.field === field);

      return index !== -1 ? index + 1 : 0;
    }
}

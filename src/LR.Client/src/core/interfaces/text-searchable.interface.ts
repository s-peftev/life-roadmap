export interface TextSearchable<T = unknown> {
  searchText: string;
  fields: T[];
}
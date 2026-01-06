export interface TextSearchable<T = unknown> {
  searchText: string;
  fields: Set<T>;
}
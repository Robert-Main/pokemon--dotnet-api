export const resources = [
  { id: "pokemon", label: "Pokémon" },
  { id: "category", label: "Categories" },
  { id: "country", label: "Countries" },
  { id: "owner", label: "Owners" },
  { id: "reviewer", label: "Reviewers" },
  { id: "review", label: "Reviews" },
] as const;

export type Resource = (typeof resources)[number]["id"];

export function isResource(value: string): value is Resource {
  return resources.some(resource => resource.id === value);
}

export interface ApiResponse<T> { message: string; data: T; }
export interface Country { id: number; name: string | null; }
export interface Category { id: number; name: string | null; }
export interface Owner { id: number; firstName: string | null; lastName: string | null; gym: string | null; countryId: number | null; country: Country | null; }
export interface Reviewer { id: number; firstName: string | null; lastName: string | null; }
export interface Review { id: number; title: string | null; text: string | null; rating: number; reviewerId: number; pokemonId: number; }
export interface Pokemon { id: number; name: string | null; birthDate: string; createdAt: string | null; reviews?: PokemonReview[]; pokemonCategories?: PokemonCategory[]; pokemonOwners?: PokemonOwner[]; }
export interface PokemonReview { id: number; title: string | null; text: string | null; rating: number; reviewer: Reviewer | null; }
export interface PokemonCategory { categoryId: number; category: Category | null; }
export interface PokemonOwner { ownerId: number; owner: Owner | null; }
export type Entity = Pokemon | Category | Country | Owner | Reviewer | Review;

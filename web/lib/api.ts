import type { ApiResponse, Category, Country, Owner, Pokemon, Review, Reviewer } from "./types";

const baseUrl = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5064/api";

export class ApiError extends Error {
    constructor(
        message: string,
        public readonly status: number,
    ) {
        super(message);
    }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
    const response = await fetch(`${baseUrl}/${path}`, {
        ...init,
        headers: { "Content-Type": "application/json", ...init?.headers },
    });
    const body = (await response.json().catch(() => null)) as ApiResponse<T> | { message?: string } | null;
    if (!response.ok) throw new ApiError(body?.message ?? "The request failed.", response.status);
    return (body as ApiResponse<T>).data;
}

interface CrudService<T extends { id: number }, TCreate, TUpdate = T> {
    list(): Promise<T[]>;
    get(id: number): Promise<T>;
    create(input: TCreate): Promise<T>;
    update(id: number, input: TUpdate): Promise<T>;
    remove(id: number): Promise<void>;
}

function createCrudService<T extends { id: number }, TCreate, TUpdate = T>(
    resource: string,
): CrudService<T, TCreate, TUpdate> {
    return {
        list: () => request<T[]>(resource),
        get: (id) => request<T>(`${resource}/${id}`),
        create: (input) => request<T>(resource, { method: "POST", body: JSON.stringify(input) }),
        update: (id, input) => request<T>(`${resource}/${id}`, { method: "PUT", body: JSON.stringify(input) }),
        remove: async (id) => {
            await request<unknown>(`${resource}/${id}`, { method: "DELETE" });
        },
    };
}

export const pokemonService = createCrudService<Pokemon, Pick<Pokemon, "name" | "birthDate">>("Pokemon");
export const categoryService = createCrudService<Category, Pick<Category, "name">>("Category");
export const countryService = createCrudService<Country, Pick<Country, "name">>("Country");
export const ownerService = createCrudService<Owner, Pick<Owner, "firstName" | "lastName" | "gym" | "countryId">>(
    "Owner",
);
export const reviewerService = createCrudService<Reviewer, Pick<Reviewer, "firstName" | "lastName">>("Reviewer");
export const reviewService = createCrudService<Review, Omit<Review, "id">>("Review");

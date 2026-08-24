"use client";

import { FormEvent, useCallback, useEffect, useMemo, useState } from "react";
import { ApiError, categoryService, countryService, ownerService, pokemonService, reviewService, reviewerService } from "../lib/api";
import type { Category, Country, Entity, Owner, Pokemon, Review, Reviewer } from "../lib/types";

type Resource = "pokemon" | "category" | "country" | "owner" | "reviewer" | "review";
type FormValues = Record<string, string>;
const resources: { id: Resource; label: string }[] = [
  { id: "pokemon", label: "Pokémon" }, { id: "category", label: "Categories" }, { id: "country", label: "Countries" },
  { id: "owner", label: "Owners" }, { id: "reviewer", label: "Reviewers" }, { id: "review", label: "Reviews" },
];
const serviceMap = { pokemon: pokemonService, category: categoryService, country: countryService, owner: ownerService, reviewer: reviewerService, review: reviewService };
const emptyForms: Record<Resource, FormValues> = {
  pokemon: { name: "", birthDate: "" }, category: { name: "" }, country: { name: "" }, owner: { firstName: "", lastName: "", gym: "", countryId: "" }, reviewer: { firstName: "", lastName: "" }, review: { title: "", text: "", rating: "1", reviewerId: "", pokemonId: "" },
};

const label = (value: string | null | undefined) => value || "—";
const person = (value: { firstName: string | null; lastName: string | null }) => `${value.firstName ?? ""} ${value.lastName ?? ""}`.trim() || "Unnamed";

export default function Dashboard() {
  const [resource, setResource] = useState<Resource>("pokemon");
  const [records, setRecords] = useState<Entity[]>([]);
  const [countries, setCountries] = useState<Country[]>([]);
  const [pokemons, setPokemons] = useState<Pokemon[]>([]);
  const [reviewers, setReviewers] = useState<Reviewer[]>([]);
  const [form, setForm] = useState<FormValues>(emptyForms.pokemon);
  const [editing, setEditing] = useState<Entity | null>(null);
  const [selectedPokemon, setSelectedPokemon] = useState<Pokemon | null>(null);
  const [pendingDelete, setPendingDelete] = useState<Entity | null>(null);
  const [deleting, setDeleting] = useState(false);
  const [loading, setLoading] = useState(true);
  const [notice, setNotice] = useState<string | null>(null);

  const loadLookups = useCallback(async () => {
    const [loadedCountries, loadedPokemons, loadedReviewers] = await Promise.all([countryService.list(), pokemonService.list(), reviewerService.list()]);
    setCountries(loadedCountries); setPokemons(loadedPokemons); setReviewers(loadedReviewers);
  }, []);
  const load = useCallback(async () => {
    setLoading(true); setNotice(null);
    try { setRecords(await serviceMap[resource].list()); await loadLookups(); }
    catch (error) { setNotice(error instanceof Error ? error.message : "Could not reach the API."); }
    finally { setLoading(false); }
  }, [resource, loadLookups]);
  useEffect(() => { setForm(emptyForms[resource]); setEditing(null); setSelectedPokemon(null); void load(); }, [resource, load]);

  const fields = useMemo(() => Object.keys(emptyForms[resource]), [resource]);
  function beginEdit(record: Entity) {
    setEditing(record);
    const values: FormValues = {};
    fields.forEach(field => { const value = (record as unknown as Record<string, unknown>)[field]; values[field] = value == null ? "" : field === "birthDate" ? String(value).slice(0, 10) : String(value); });
    setForm(values);
  }
  function resetForm() { setEditing(null); setForm(emptyForms[resource]); }
  function payload() {
    const numberFields = new Set(["rating", "reviewerId", "pokemonId"]);
    return Object.fromEntries(Object.entries(form).map(([key, value]) => [key, key === "countryId" ? (value ? Number(value) : null) : numberFields.has(key) ? Number(value) : value]));
  }
  async function submit(event: FormEvent) {
    event.preventDefault(); setNotice(null);
    try {
      const service = serviceMap[resource] as never;
      const input = resource === "pokemon" && editing ? { ...payload(), createdAt: (editing as Pokemon).createdAt } : payload();
      if (editing) await (service as { update(id: number, input: object): Promise<Entity> }).update(editing.id, input);
      else await (service as { create(input: object): Promise<Entity> }).create(payload());
      resetForm(); setNotice(`${editing ? "Updated" : "Created"} successfully.`); await load();
    } catch (error) { setNotice(error instanceof ApiError ? error.message : "Unable to save this record."); }
  }
  async function confirmDelete() {
    if (!pendingDelete || deleting) return;
    setDeleting(true); setNotice(null);
    try {
      await (serviceMap[resource] as { remove(id: number): Promise<void> }).remove(pendingDelete.id);
      setPendingDelete(null); setNotice("Deleted successfully."); await load();
    } catch (error) { setNotice(error instanceof Error ? error.message : "Unable to delete this record."); }
    finally { setDeleting(false); }
  }
  async function viewPokemon(id: number) { try { setSelectedPokemon(await pokemonService.get(id)); } catch { setNotice("Unable to load Pokémon details."); } }

  return <main>
    <aside><div className="brand"><span>◈</span><div><strong>Pokédex</strong><small>ADMIN CONSOLE</small></div></div><nav>{resources.map(item => <button key={item.id} className={resource === item.id ? "active" : ""} onClick={() => setResource(item.id)}>{item.label}</button>)}</nav><p className="api-note">API: {process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5064/api"}</p></aside>
    <section className="content"><header><div><p className="eyebrow">DATA MANAGEMENT</p><h1>{resources.find(item => item.id === resource)?.label}</h1></div><button className="secondary" onClick={() => void load()}>↻ Refresh</button></header>
      {notice && <p className="notice">{notice}</p>}
      <div className="layout"><section className="panel form-panel"><h2>{editing ? "Edit record" : `New ${resources.find(item => item.id === resource)?.label.slice(0, -1) ?? resource}`}</h2><form onSubmit={submit}>{fields.map(field => <Field key={field} name={field} value={form[field] ?? ""} onChange={value => setForm(current => ({ ...current, [field]: value }))} countries={countries} pokemons={pokemons} reviewers={reviewers} />)}<div className="actions"><button type="submit">{editing ? "Save changes" : "Create record"}</button>{editing && <button type="button" className="secondary" onClick={resetForm}>Cancel</button>}</div></form></section>
        <section className="panel table-panel"><div className="panel-title"><h2>All records</h2><span>{records.length}</span></div>{loading ? <p className="muted">Loading…</p> : records.length === 0 ? <p className="muted">No records yet.</p> : <div className="record-list">{records.map(record => <article key={record.id} className="record"><div><strong>{display(record, resource)}</strong><small>{summary(record, resource)}</small></div><div className="row-actions">{resource === "pokemon" && <button className="link" onClick={() => void viewPokemon(record.id)}>Details</button>}<button className="link" onClick={() => beginEdit(record)}>Edit</button><button className="link danger" onClick={() => setPendingDelete(record)}>Delete</button></div></article>)}</div>}</section></div>
      {selectedPokemon && <PokemonDetails pokemon={selectedPokemon} onClose={() => setSelectedPokemon(null)} />}
      {pendingDelete && <DeleteConfirmation record={pendingDelete} resource={resource} deleting={deleting} onCancel={() => setPendingDelete(null)} onConfirm={() => void confirmDelete()} />}
    </section>
  </main>;
}

function Field({ name, value, onChange, countries, pokemons, reviewers }: { name: string; value: string; onChange(value: string): void; countries: Country[]; pokemons: Pokemon[]; reviewers: Reviewer[] }) {
  const title = name.replace(/([A-Z])/g, " $1").replace(/^./, c => c.toUpperCase());
  if (name === "countryId" || name === "pokemonId" || name === "reviewerId") { const options = name === "countryId" ? countries.map(item => [item.id, label(item.name)]) : name === "pokemonId" ? pokemons.map(item => [item.id, label(item.name)]) : reviewers.map(item => [item.id, person(item)]); return <label>{title}<select value={value} required={name !== "countryId"} onChange={e => onChange(e.target.value)}><option value="">{name === "countryId" ? "No country" : `Select ${title.toLowerCase()}`}</option>{options.map(([id, text]) => <option key={id} value={id}>{text}</option>)}</select></label>; }
  if (name === "text") return <label>{title}<textarea value={value} required onChange={e => onChange(e.target.value)} /></label>;
  return <label>{title}<input type={name === "birthDate" ? "date" : name === "rating" ? "number" : "text"} min={name === "rating" ? "1" : undefined} max={name === "rating" ? "5" : undefined} value={value} required={name !== "gym"} onChange={e => onChange(e.target.value)} /></label>;
}
function display(record: Entity, resource: Resource) { if (resource === "owner" || resource === "reviewer") return person(record as Owner | Reviewer); if (resource === "review") return label((record as Review).title); return label((record as Pokemon | Category | Country).name); }
function summary(record: Entity, resource: Resource) { if (resource === "pokemon") return `Born ${new Date((record as Pokemon).birthDate).toLocaleDateString()}`; if (resource === "owner") return `${label((record as Owner).gym)} · ${label((record as Owner).country?.name)}`; if (resource === "review") return `${(record as Review).rating}/5 · Pokémon #${(record as Review).pokemonId}`; return `ID #${record.id}`; }
function PokemonDetails({ pokemon, onClose }: { pokemon: Pokemon; onClose(): void }) { return <div className="modal-backdrop" onMouseDown={onClose}><section className="modal" onMouseDown={event => event.stopPropagation()}><button className="close" onClick={onClose}>×</button><p className="eyebrow">POKÉMON PROFILE</p><h2>{label(pokemon.name)}</h2><p className="muted">Born {new Date(pokemon.birthDate).toLocaleDateString()}</p><h3>Categories</h3><p>{pokemon.pokemonCategories?.map(item => item.category?.name).filter(Boolean).join(", ") || "None"}</p><h3>Owners</h3><p>{pokemon.pokemonOwners?.map(item => item.owner && person(item.owner)).filter(Boolean).join(", ") || "None"}</p><h3>Reviews</h3>{pokemon.reviews?.length ? pokemon.reviews.map(review => <article className="review" key={review.id}><strong>{label(review.title)} · {review.rating}/5</strong><small>by {review.reviewer ? person(review.reviewer) : "Unknown"}</small><p>{label(review.text)}</p></article>) : <p>No reviews.</p>}</section></div>; }
function DeleteConfirmation({ record, resource, deleting, onCancel, onConfirm }: { record: Entity; resource: Resource; deleting: boolean; onCancel(): void; onConfirm(): void }) { return <div className="modal-backdrop" onMouseDown={deleting ? undefined : onCancel}><section className="modal confirm-modal" role="alertdialog" aria-modal="true" aria-labelledby="delete-title" aria-describedby="delete-description" onMouseDown={event => event.stopPropagation()}><p className="eyebrow">CONFIRM DELETION</p><h2 id="delete-title">Delete {display(record, resource)}?</h2><p id="delete-description" className="muted">This action cannot be undone.</p><div className="actions confirm-actions"><button type="button" className="secondary" onClick={onCancel} disabled={deleting}>Cancel</button><button type="button" className="delete-confirm" onClick={onConfirm} disabled={deleting}>{deleting ? "Deleting…" : "Delete record"}</button></div></section></div>; }

import { notFound } from "next/navigation";
import Dashboard from "../components/Dashboard";
import { isResource, resources } from "../../lib/resources";

export function generateStaticParams() {
  return resources.map(({ id }) => ({ resource: id }));
}

export default async function ResourcePage({ params }: { params: Promise<{ resource: string }> }) {
  const { resource } = await params;

  if (!isResource(resource)) notFound();

  return <Dashboard initialResource={resource} />;
}

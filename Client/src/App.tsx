import { useState } from "react";
import { useSizzlingProduct } from "./hooks/useSizzlingProduct";
import type { QueryMode } from "./types";

export default function App() {
  const [mode, setMode] = useState<QueryMode>("daily");
  const [from, setFrom] = useState("2026-04-21");
  const [to, setTo] = useState("2026-04-23");
  const { product, loading, error, query } = useSizzlingProduct();

  function handleSubmit(e: React.SyntheticEvent) {
    e.preventDefault();
    query(mode, from, to);
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-sm bg-white border border-gray-200 rounded-lg p-6">

        <h1 className="text-lg font-semibold text-gray-900 mb-1">Sizzling Hot Products</h1>
        <p className="text-sm text-gray-400 mb-6">Top selling product by day or date range</p>

        {/* Mode toggle */}
        <div className="flex gap-2 mb-5">
          {(["daily", "period"] as QueryMode[]).map((m) => (
            <button
              key={m}
              type="button"
              onClick={() => setMode(m)}
              className={`px-3 py-1.5 text-xs rounded font-medium border transition-colors ${
                mode === m
                  ? "bg-gray-900 text-white border-gray-900"
                  : "bg-white text-gray-500 border-gray-200 hover:border-gray-400"
              }`}
            >
              {m === "daily" ? "Single Day" : "Date Range"}
            </button>
          ))}
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="space-y-3">
          <div>
            <label className="block text-xs text-gray-500 mb-1">
              {mode === "daily" ? "Date" : "From"}
            </label>
            <input
              type="date"
              value={from}
              onChange={(e) => setFrom(e.target.value)}
              required
              className="w-full border border-gray-200 rounded px-3 py-2 text-sm text-gray-800 focus:outline-none focus:border-gray-400"
            />
          </div>

          {mode === "period" && (
            <div>
              <label className="block text-xs text-gray-500 mb-1">To</label>
              <input
                type="date"
                value={to}
                onChange={(e) => setTo(e.target.value)}
                required
                className="w-full border border-gray-200 rounded px-3 py-2 text-sm text-gray-800 focus:outline-none focus:border-gray-400"
              />
            </div>
          )}

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-gray-900 hover:bg-gray-700 disabled:opacity-40 text-white text-sm font-medium py-2 rounded transition-colors"
          >
            {loading ? "Loading..." : "Search"}
          </button>
        </form>

        {error && (
          <p className="mt-4 text-sm text-red-500">{error}</p>
        )}

        {product && (
          <div className="mt-4 p-4 bg-gray-50 border border-gray-200 rounded">
            <p className="text-xs text-gray-400 mb-1">Top product</p>
            <p className="text-sm font-semibold text-gray-900">{product}</p>
          </div>
        )}
      </div>
    </div>
  );
}

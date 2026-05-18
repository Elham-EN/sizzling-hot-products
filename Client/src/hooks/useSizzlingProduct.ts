import { useState } from 'react'
import axios from 'axios'
import type { ApiResult, QueryMode } from '../types'

const API_BASE = 'https://localhost:5001/api/sizzlinghotproducts'

export function useSizzlingProduct() {
  const [product, setProduct] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function query(mode: QueryMode, from: string, to?: string) {
    setLoading(true)
    setError(null)
    setProduct(null)

    try {
      const url =
        mode === 'daily'
          ? `${API_BASE}/daily?date=${from}`
          : `${API_BASE}/period?from=${from}&to=${to}`

      const { data } = await axios.get<ApiResult>(url)
      setProduct(data.product)
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (err.response?.status === 404) {
          setError('No sales found for that date or period.')
        } else if (err.response?.status === 400) {
          setError(err.response.data ?? 'Invalid date range.')
        } else if (!err.response) {
          setError('Could not reach the API. Make sure the backend is running.')
        } else {
          setError('Something went wrong.')
        }
      }
    } finally {
      setLoading(false)
    }
  }

  return { product, loading, error, query }
}

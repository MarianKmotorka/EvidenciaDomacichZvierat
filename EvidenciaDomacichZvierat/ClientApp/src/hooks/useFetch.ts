import { useCallback, useEffect, useState } from 'react'

interface IUseFetchParameters {
  url: string
}

const useFetch = <T>({ url }: IUseFetchParameters) => {
  const [data, setData] = useState<T>()
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<any>()

  const fetchData = useCallback(async () => {
    const res = await fetch(url)
    const json = await res.json()

    if (res.ok) setData(json)
    else setError(json)
    setLoading(false)
  }, [url])

  useEffect(() => {
    fetchData()
  }, [fetchData])

  return { data, loading, error }
}

export default useFetch

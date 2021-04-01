import { useParams } from 'react-router'

const MajitelDetail = () => {
  const { id } = useParams<{ id: string }>()

  return <div>Detail: {id}</div>
}

export default MajitelDetail

import { CardContent, CircularProgress, Dialog } from '@material-ui/core'
import useFetch from '../../hooks/useFetch'
import NameValueRow from '../../components/NameValueRow'

interface IProps {
  majitelIds: number[]
  onClose: () => void
}

interface IMajitelStatistics {
  priemernyPocetZvieratNaMajitela: number
  priemernyVekZvierat: number
  pocetZvieratOdMajitelov: number
}

const MajitelStatisticsDialog = ({ majitelIds, onClose }: IProps) => {
  const { data, loading, error } = useFetch<IMajitelStatistics>({
    url: '/api/majitel/statistics',
    method: 'POST',
    body: { ids: majitelIds }
  })

  if (loading)
    return (
      <Dialog open onClose={onClose}>
        <CardContent>
          <CircularProgress />
        </CardContent>
      </Dialog>
    )

  if (error)
    return (
      <Dialog open onClose={onClose}>
        <CardContent>
          <p>Error</p>
        </CardContent>
      </Dialog>
    )

  const { pocetZvieratOdMajitelov, priemernyVekZvierat, priemernyPocetZvieratNaMajitela } = data!

  return (
    <Dialog open onClose={onClose}>
      <CardContent>
        <NameValueRow name='Pocet oznacenych majitelov' value={majitelIds.length} />

        <NameValueRow
          name='Priemerny vek zvierat'
          value={`${priemernyVekZvierat.toFixed(1)} rokov`}
        />

        <NameValueRow
          name='Priemerny pocet zvierat na majitela'
          value={priemernyPocetZvieratNaMajitela.toFixed(1)}
        />

        <NameValueRow name='Pocet zvierat od majitelov' value={pocetZvieratOdMajitelov} />
      </CardContent>
    </Dialog>
  )
}

export default MajitelStatisticsDialog

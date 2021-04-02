import { CardContent, CircularProgress, Dialog } from '@material-ui/core'
import useFetch from '../../hooks/useFetch'
import NameValueRow from '../NameValueRow'

interface IProps {
  majitelIds: number[]
  onClose: () => void
}

interface IMajitelStatistics {
  priemernyPocetZvieratNaMajitela: number
  priemernyVekZvieratNaMajitela: number
  pocetZvieratOdMajitelov: number
}

const MajitelStatisticsDialog = ({ majitelIds, onClose }: IProps) => {
  const { data, loading, error } = useFetch<IMajitelStatistics>({
    url: '/api/majitel/statistics',
    method: 'POST',
    body: { ids: majitelIds }
  })

  return (
    <Dialog open onClose={onClose}>
      <CardContent>
        {loading && <CircularProgress />}

        {error && <p>ERROR</p>}

        {!loading && !error && (
          <>
            <NameValueRow name='Pocet oznacenych majitelov' value={majitelIds.length} />

            <NameValueRow
              name='Priemerny pocet zvierat na majitela'
              value={data!.priemernyPocetZvieratNaMajitela.toFixed(1)}
            />

            <NameValueRow
              name='Priemerny vek zvierat na majitela'
              value={data!.priemernyVekZvieratNaMajitela.toFixed(1)}
            />

            <NameValueRow name='Pocet zvierat od majitelov' value={data!.pocetZvieratOdMajitelov} />
          </>
        )}
      </CardContent>
    </Dialog>
  )
}

export default MajitelStatisticsDialog

import { memo, useState } from 'react'
import moment from 'moment'
import { Fastfood, Pets } from '@material-ui/icons'
import { Box, Button, Card, CardActions, CardContent, Dialog, MenuItem } from '@material-ui/core'

import { IPes } from '../../../types'
import NameValueRow from '../../NameValueRow'
import { getFormattedAge } from '../../../utils'

interface IProps {
  data: IPes
  onNakrmit: (zvieraId: number) => Promise<void>
}

const Pes = memo(({ data, onNakrmit }: IProps) => {
  const [open, setOpen] = useState(false)

  return (
    <>
      <Card elevation={0}>
        <MenuItem onClick={() => setOpen(true)}>
          <CardContent>
            <Box display='flex' alignItems='center' gridGap='10px'>
              <Pets color='primary' />
              <h4>{data.meno}</h4>
            </Box>

            <p>(Pes)</p>
          </CardContent>
        </MenuItem>
      </Card>

      <Dialog open={open} onClose={() => setOpen(false)}>
        <Box minWidth={400}>
          <CardContent>
            <NameValueRow name='Meno' value={data.meno} />
            <NameValueRow name='Vek' value={getFormattedAge(data.datumNarodenia)} />
            <NameValueRow
              name='Datum narodenia'
              value={moment(data.datumNarodenia).format('DD.MM.YYYY')}
            />
            <NameValueRow name='Predpokladany vzrast' value={data.predpokladanyVzrastCm + 'cm'} />
            <NameValueRow name='Uroven vycviku' value={data.urovenVycviku + '/10'} />
            <NameValueRow name='Pocet krmeni' value={data.pocetKrmeni} />

            <CardActions>
              <Button
                startIcon={<Fastfood />}
                variant='contained'
                color='primary'
                onClick={async () => await onNakrmit(data.id)}
              >
                Nakrmit
              </Button>
            </CardActions>
          </CardContent>
        </Box>
      </Dialog>
    </>
  )
})

export default Pes

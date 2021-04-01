import {
  AppBar,
  Box,
  Container,
  createStyles,
  makeStyles,
  Theme,
  Typography
} from '@material-ui/core'

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      [theme.breakpoints.down('xs')]: {
        fontSize: 20,
        padding: '8px 0'
      }
    }
  })
)

const Navbar = () => {
  const styles = useStyles()

  return (
    <AppBar position='sticky'>
      <Container maxWidth='md'>
        <Box paddingY='10px'>
          <Typography className={styles.root} variant='h4'>
            Evidencia domacich zvierat
          </Typography>
        </Box>
      </Container>
    </AppBar>
  )
}

export default Navbar

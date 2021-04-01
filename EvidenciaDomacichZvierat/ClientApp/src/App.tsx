import { AppBar, Box, Button, Container, CssBaseline, Typography } from '@material-ui/core'
import { Route, Switch } from 'react-router'
import Navbar from './components/Navbar'
import MajitelDetail from './pages/MajitelDetail'
import MajitelList from './pages/Majitelia'

function App() {
  return (
    <>
      <CssBaseline />

      <Navbar />

      <Switch>
        <Route path='/majitelia/:id' component={MajitelDetail} />
        <Route path='/' component={MajitelList} />
      </Switch>
    </>
  )
}

export default App

import { Route, Switch } from 'react-router'
import { CssBaseline } from '@material-ui/core'

import Navbar from './components/Navbar'
import Majitelia from './pages/Majitelia'
import MajitelDetail from './pages/MajitelDetail'

function App() {
  return (
    <>
      <CssBaseline />

      <Navbar />

      <Switch>
        <Route path='/majitelia/:id' component={MajitelDetail} />
        <Route path='/' component={Majitelia} />
      </Switch>
    </>
  )
}

export default App

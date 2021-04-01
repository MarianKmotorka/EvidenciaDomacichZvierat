import styled from 'styled-components'

interface IProps {
  name: string
  value: string | number
}

const Wrapper = styled.div`
  background-color: ${({ theme }) => theme.background.paper};
  padding: 8px 12px;

  p {
    font-weight: bold;
    margin: 5px 0;
  }
`

const NameValueRow = ({ name, value }: IProps) => {
  return (
    <Wrapper>
      <p>{name}</p>
      <span>{value}</span>
    </Wrapper>
  )
}

export default NameValueRow

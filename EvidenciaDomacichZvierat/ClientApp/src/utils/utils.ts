import moment from 'moment'

export const getFormattedAge = (birthDate: Date | string) => {
  const { years, months, date } = moment(moment().diff(birthDate)).toObject()
  return `${years - 1970}rokov, ${months}mesiacov, ${date}dni`
}

import { formatDate } from "../../utils/global";

const Education = ({ education }) => {
  return (
    <div>
      {education?.map((e) => (
        <div key={e.id}>
          <p>
            &#127891; {e.current ? "Studies" : "Studied"} <b>{e.degree}</b> of <b>{e.fieldofstudy}</b> at <b>{e.school}</b>
          </p>
          <small>
            from {formatDate(e.from)} to {e.current ? "Current" : formatDate(e.to)}
          </small>
        </div>
      ))}
    </div>
  );
};

export default Education;

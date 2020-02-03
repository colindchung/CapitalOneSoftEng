SELECT last_name, salary + NVL(commission_pct, 0), 
	job_id, e.department_id
	/* Select all employees whose compensation is
	greater than that of Pataballa.*/
FROM employees e, departments d
    /*The DEPARTMENTS table is used to get the department name.*/
WHERE e.department_id = d.department_id
AND salary + NVL(commission_pct,0) >   /* Subquery:       */
	(SELECT salary + NVL(commission_pct,0)
                /* total compensation is salar + commission_pct */
    FROM employees 
    WHERE last_name = 'Smith');

SELECT last_name,                    -- select the name
	salary + NVL(commission_pct, 0),-- total compensation
	job_id,                         -- job
	e.department_id                 -- and department
FROM employees e,                 -- of all employees
    departments d
WHERE e.department_id = d.department_id
	AND salary + NVL(commission_pct, 0) >  -- whose compensation 
                                        -- is greater than
    (SELECT salary + NVL(commission_pct,0)  -- the compensation
FROM employees 
WHERE last_name = 'Smith')        -- of Smith.
;